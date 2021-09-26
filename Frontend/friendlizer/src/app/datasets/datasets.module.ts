import { DatasetsComponent } from './datasets.component';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';

@NgModule({
    imports: [MatButtonModule, MatIconModule, MatDialogModule, HttpClientModule, MatProgressSpinnerModule, CommonModule, MatSnackBarModule, MatTableModule],
    exports: [DatasetsComponent],
    declarations: [DatasetsComponent],
    providers: [],
})
export class DatasetsModule { }
