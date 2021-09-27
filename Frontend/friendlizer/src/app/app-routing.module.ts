import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DatasetsComponent } from './datasets/datasets.component';
import { StatisticsComponent } from './statistics/statistics.component';

const routes: Routes = [
    {
        path: '',
        component: DatasetsComponent,
        data: {
            title: 'Datasets Management'
        }
    },
    {
        path: 'statistics',
        component: StatisticsComponent,
        data: {
            title: 'Statistics'
        }
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
