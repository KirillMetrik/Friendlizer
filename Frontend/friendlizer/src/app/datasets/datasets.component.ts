import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';

@Component({
    selector: 'datasets',
    templateUrl: 'datasets.component.html',
    styleUrls: ['./datasets.component.less']
})
export class DatasetsComponent implements OnInit {
    @ViewChild('txtInputFile') txtInputFile?: ElementRef<HTMLInputElement>;

    constructor() { }

    ngOnInit() { }

    txtInputChange() {
        const files = this.txtInputFile?.nativeElement.files;
        alert('Upload file here');
        // TODO: start upload here
    }
}