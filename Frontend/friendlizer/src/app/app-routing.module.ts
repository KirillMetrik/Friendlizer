import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DatasetsComponent } from './datasets/datasets.component';
import { StatisticsComponent } from './statistics/statistics.component';

const routes: Routes = [
    {
        path: '',
        component: DatasetsComponent
    },
    {
        path: 'statistics',
        component: StatisticsComponent
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
