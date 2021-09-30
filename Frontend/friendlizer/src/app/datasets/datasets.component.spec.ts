import { TestBed } from '@angular/core/testing';
import { DatasetsComponent } from './datasets.component';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule } from '@angular/common';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

describe('DatasetsComponent', () => {
    let matSnackBar: jasmine.SpyObj<MatSnackBar>;

    beforeEach(async () => {
        matSnackBar = jasmine.createSpyObj(MatSnackBar, ['open']);
        matSnackBar.open.and.stub();

        await TestBed.configureTestingModule({
            imports: [
                HttpClientTestingModule,
                MatButtonModule, MatIconModule, MatDialogModule, MatProgressSpinnerModule, CommonModule, MatSnackBarModule, MatTableModule
            ],
            providers: [
                {
                    provide: MatSnackBar,
                    useValue: matSnackBar
                }
            ],
            declarations: [
                DatasetsComponent
            ],
        }).compileComponents();
    });

    it('should render existing datasets in table', () => {
        const fixture = TestBed.createComponent(DatasetsComponent);
        const http = TestBed.inject(HttpTestingController);

        fixture.detectChanges();
        const testReq = http.expectOne(req => req.method === 'GET' && req.url.search('/api/FriendsSets') !== -1);
        testReq.flush([{
            id: 1,
            filename: 'aaa.txt'
        }]);

        fixture.detectChanges();
        const el: HTMLElement = fixture.nativeElement;
        const tbody = el.querySelector('table tbody');

        expect(tbody.querySelectorAll('tr').length).toBe(1);
        expect(tbody.querySelector('td.cdk-column-id').textContent).toBe('1');
        expect(tbody.querySelector('td.cdk-column-filename').textContent).toBe('aaa.txt');
    });

    it('should show/hide progress indicator correctly', () => {
        const fixture = TestBed.createComponent(DatasetsComponent);
        const http = TestBed.inject(HttpTestingController);

        fixture.detectChanges();
        const testReq = http.expectOne(req => req.method === 'GET' && req.url.search('/api/FriendsSets') !== -1);

        const el: HTMLElement = fixture.nativeElement;
        expect(el.querySelector('mat-spinner')).not.toBeNull();
        testReq.flush([]);
        fixture.detectChanges();
        expect(el.querySelector('mat-spinner')).toBeNull();
    });

    it('should show snack about successful import', () => {
        const fixture = TestBed.createComponent(DatasetsComponent);
        const http = TestBed.inject(HttpTestingController);

        fixture.detectChanges();
        const dt = new DataTransfer();
        dt.items.add(new File([''], 'aaa.txt'));
        fixture.componentInstance.txtInputFile.nativeElement.files = dt.files;

        fixture.componentInstance.txtInputChange();
        const testReq = http.expectOne(req => req.method==='POST' && req.url.search('/api/FriendsSets') !== -1);
        testReq.flush({
            id: 1,
            filename: 'aaa.txt',
            imported: 1234
        });
        fixture.detectChanges();

        expect(matSnackBar.open).toHaveBeenCalledOnceWith('Successfully imported 1234 records', jasmine.any(String), jasmine.any(Object));
    });

    it('should show snack about error during import', () => {
        const fixture = TestBed.createComponent(DatasetsComponent);
        const http = TestBed.inject(HttpTestingController);

        fixture.detectChanges();
        const dt = new DataTransfer();
        dt.items.add(new File([''], 'aaa.txt'));
        fixture.componentInstance.txtInputFile.nativeElement.files = dt.files;

        fixture.componentInstance.txtInputChange();
        const testReq = http.expectOne(req => req.method === 'POST' && req.url.search('/api/FriendsSets') !== -1);
        testReq.error(<any>'Something happened!');
        fixture.detectChanges();

        expect(matSnackBar.open).toHaveBeenCalledOnceWith('Something happened!', jasmine.any(String), jasmine.any(Object));
    });
});
