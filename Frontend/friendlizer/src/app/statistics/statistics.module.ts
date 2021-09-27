import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { StatisticsComponent } from './statistics.component';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
    imports: [CommonModule, MatSelectModule, MatButtonModule, MatCardModule, MatProgressSpinnerModule],
    exports: [],
    declarations: [StatisticsComponent],
    providers: [],
})
export class StatisticsModule { }
