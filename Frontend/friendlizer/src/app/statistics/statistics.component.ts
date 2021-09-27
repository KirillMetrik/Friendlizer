import { Dataset } from '../datasets/dataset.model';
import { Stats } from './stats.model';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { finalize, map, mergeMap, tap } from 'rxjs/operators';

@Component({
    selector: 'statistics',
    templateUrl: 'statistics.component.html',
    styleUrls: ['./statistics.component.less']
})
export class StatisticsComponent implements OnInit {
    selectedDs = '';
    datasets$ = this.httpClient.get<Dataset[]>('https://localhost:44351/api/FriendsSets');

    selectedDs$ = new Subject<string>();
    stats$ = this.selectedDs$.pipe(
        tap(() => this.isBusy = true),
        mergeMap(ds => this.httpClient.get<Stats>(`https://localhost:44351/api/FriendsSets/${ds}/stats`).pipe(finalize(() => this.isBusy = false)))
    );
    isBusy = false;

    constructor(private httpClient: HttpClient) { }

    ngOnInit() { }

    loadStats() {
        this.selectedDs$.next(this.selectedDs);
    }
}