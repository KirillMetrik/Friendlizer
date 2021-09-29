import { AfterViewInit, Component, ElementRef, Input, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { Relation } from './stats.model';
import { Edge, Network, Node } from 'vis';
import { PageEvent } from '@angular/material/paginator';

@Component({
    selector: 'relations-graph',
    templateUrl: 'relations-graph.component.html',
    styleUrls: ['./relations-graph.component.less']
})
export class RelationsGraphComponent implements AfterViewInit {

    @Input() relations: Relation[];
    @ViewChild('chartContainer') chartContainer: ElementRef<HTMLElement>;

    ngAfterViewInit() {
        this.refreshChart();
    }

    ngOnChanges(changes: SimpleChanges) {
        this.refreshChart();
    }

    onPageChange(ev: PageEvent) {
        this.refreshChart(ev.pageIndex, ev.pageSize);
    }

    private refreshChart(page = 0, pageSize = 50) {
        if (!this.chartContainer) {
            return;
        }


        const nodes: Node[] = [];
        const nodesMap = new Map<number, Node>();
        const edges: Edge[] = [];
        const startIndex = page * pageSize

        for (let index = startIndex; index < startIndex + pageSize; index++){
            this.ensureNode(nodes, nodesMap, this.relations[index].firstPersonId);
            this.ensureNode(nodes, nodesMap, this.relations[index].secondPersonId);
            edges.push({
                from: this.relations[index].firstPersonId,
                to: this.relations[index].secondPersonId
            });
        }

        const network = new Network(this.chartContainer.nativeElement, { nodes, edges }, {
            nodes: {
                shape: "dot",
                font: {
                    size: 12,
                    face: "Tahoma",
                }
            },
            physics: {
                stabilization: {
                    enabled: true,
                    iterations: 100,
                    updateInterval: 5
                }
            }
        });
        network.on("stabilizationIterationsDone", function () {
            network.setOptions({ physics: false });
        });
    }

    private ensureNode(nodes: Node[], nodesMap: Map<number, Node>, id: number) {
        const exists = nodesMap.has(id);
        if (!exists) {
            nodesMap.set(id, {
                id, label: '' + id
            });
            nodes.push(nodesMap.get(id));
        }
    }
}