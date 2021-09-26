import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable, Subject } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { Dataset, ImportResult } from './dataset.model';

@Component({
    selector: 'datasets',
    templateUrl: 'datasets.component.html',
    styleUrls: ['./datasets.component.less']
})
export class DatasetsComponent implements OnInit, OnDestroy {
    @ViewChild('txtInputFile') txtInputFile?: ElementRef<HTMLInputElement>;
    isBusy = false;
    tableRows: Dataset[] = [];
    setsRefresher$ = new Subject<Dataset[]>();
    displayedColumns = ['id', 'filename'];

    constructor(private httpClient: HttpClient, private snackBar: MatSnackBar) { }

    ngOnInit() {
        this.setsRefresher$.pipe(switchMap(() => {
            this.isBusy = true;
            return this.httpClient.get<Dataset[]>('https://localhost:44351/api/FriendsSets');
        })).subscribe(data => {
            this.tableRows = data;
            this.isBusy = false;
        }, () => this.isBusy = false);

        this.setsRefresher$.next();
    }

    ngOnDestroy() {
        this.setsRefresher$.unsubscribe();
    }

    txtInputChange() {
        const files = this.txtInputFile?.nativeElement.files;
        if (!files || !files.length) {
            return;
        }

        this.isBusy = true;
        const formData = new FormData();
        formData.append('file', files[0]);
        const subscr = this.httpClient.post<ImportResult>('https://localhost:44351/api/FriendsSets', formData)
            .subscribe(resp => {
                subscr.unsubscribe();
                this.isBusy = false;
                this.snackBar.open(`Successfully imported ${resp.imported} records`, 'Close', {
                    horizontalPosition: 'right',
                    verticalPosition: 'bottom',
                    duration: 5000
                });
                this.setsRefresher$.next();
            }, (error: HttpErrorResponse) => {
                subscr.unsubscribe();
                this.isBusy = false;
                const msg = typeof(error.error)==='string' ? error.error : error.message;
                this.snackBar.open(msg, 'Close', {
                    horizontalPosition: 'right',
                    verticalPosition: 'bottom',
                    duration: 5000
                });
            });
    }
}