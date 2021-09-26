import { NgModule } from '@angular/core';
import { DatasetsComponent } from './datasets.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';

@NgModule({
    imports: [MatButtonModule, MatIconModule, MatDialogModule],
    exports: [DatasetsComponent],
    declarations: [DatasetsComponent],
    providers: [],
})
export class DatasetsModule { }
