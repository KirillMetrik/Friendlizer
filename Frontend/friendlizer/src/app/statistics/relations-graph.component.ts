import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { Edge, Node } from '@swimlane/ngx-graph';
import { Relation } from './stats.model';

@Component({
    selector: 'relations-graph',
    templateUrl: 'relations-graph.component.html'
})
export class RelationsGraphComponent implements OnInit {

    @Input() relations: Relation[];

    graphData: {
        links: Edge[],
        nodes: Node[]
    };

    constructor() { }

    ngOnInit() { }

    ngOnChanges(changes: SimpleChanges) {
        this.convertData();
    }

    private convertData() {
        this.graphData = { links: [], nodes: [] };

        const nodesMap = new Map<number, string>();
        this.relations.forEach(r => {
            nodesMap.set(r.firstPersonId, '' + r.firstPersonId);
            nodesMap.set(r.secondPersonId, '' + r.secondPersonId);
            this.graphData.links.push({
                source: '' + r.firstPersonId,
                target: '' + r.secondPersonId
            });
        });
        nodesMap.forEach(n => this.graphData.nodes.push({
            id: n,
            label: ''
        }));

        return this.graphData;
    }
}