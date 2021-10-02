import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { StatisticsComponent } from './statistics.component';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('StatisticsComponent', () => {

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [
                BrowserAnimationsModule,
                HttpClientTestingModule,
                CommonModule, MatSelectModule, MatButtonModule, MatCardModule, MatProgressSpinnerModule, MatPaginatorModule
            ],
            declarations: [
                StatisticsComponent
            ],
        }).compileComponents();
    });

    it('should disable "Show" button when dataset is not selected', () => {
        const fixture = TestBed.createComponent(StatisticsComponent);
        fixture.detectChanges();

        const el: HTMLElement = fixture.nativeElement;
        const disabledShowBtn = el.querySelector('button[mat-raised-button][disabled]');

        expect(disabledShowBtn).not.toBeNull();
        expect(disabledShowBtn).toBeDefined();
    });

    it('should enable "Show" button when dataset is selected', () => {
        const fixture = TestBed.createComponent(StatisticsComponent);
        const http = TestBed.inject(HttpTestingController);

        fixture.detectChanges();
        fixture.componentInstance.selectedDs = '1';
        const testReq = http.expectOne(req => req.method === 'GET' && req.url.search('/api/FriendsSets') !== -1);
        testReq.flush([{
            id: 1,
            filename: 'aaa.txt'
        }]);
        fixture.detectChanges();

        const el: HTMLElement = fixture.nativeElement;
        const enabledShowBtn = el.querySelector('button[mat-raised-button][ng-reflect-disabled="false"]');

        expect(enabledShowBtn).not.toBeNull();
        expect(enabledShowBtn).toBeDefined();
    });

    it('should load statistics and relations when dataset is selected', () => {
        const fixture = TestBed.createComponent(StatisticsComponent);
        const http = TestBed.inject(HttpTestingController);

        fixture.detectChanges();
        fixture.componentInstance.selectedDs = '1';
        fixture.componentInstance.loadStats();

        const testReqStats = http.expectOne(req => req.method === 'GET' && req.url.search('/api/FriendsSets/1/stats') !== -1);
        const testReqRelations = http.expectOne(req => req.method === 'GET' && req.url.search('/api/FriendsSets/1/relations') !== -1);

        testReqStats.flush({
            totalUsers: 25,
            avgFriendsCount: 4
        });
        testReqRelations.flush([
            {
                friendsSetId: 1,
                firstPersonId: 0,
                secondPersonId: 1
            },
            {
                friendsSetId: 1,
                firstPersonId: 0,
                secondPersonId: 2
            },
            {
                friendsSetId: 1,
                firstPersonId: 1,
                secondPersonId: 2
            },
            {
                friendsSetId: 1,
                firstPersonId: 2,
                secondPersonId: 3
            }
        ]);

        fixture.detectChanges();
        const el: HTMLElement = fixture.nativeElement;

        const p = el.querySelectorAll('button[mat-raised-button] + mat-card mat-card-content p');
        expect(p).toBeDefined();
        expect(p.length).toBe(2);
        expect(p[0].textContent).toBe('Total Users: 25');
        expect(p[1].textContent).toBe('Average Number of Friends For Users: 4');
    });
});
