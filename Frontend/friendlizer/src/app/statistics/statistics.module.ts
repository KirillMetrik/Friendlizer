import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { StatisticsComponent } from './statistics.component';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RelationsGraphComponent } from './relations-graph.component';
import { NgxGraphModule } from '@swimlane/ngx-graph';

@NgModule({
    imports: [CommonModule, MatSelectModule, MatButtonModule, MatCardModule, MatProgressSpinnerModule, NgxGraphModule],
    exports: [],
    declarations: [StatisticsComponent, RelationsGraphComponent],
    providers: [],
})
export class StatisticsModule { }
