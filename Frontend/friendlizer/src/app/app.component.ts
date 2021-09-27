import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Component } from '@angular/core';
import { ActivatedRoute, Router, RoutesRecognized } from '@angular/router';
import { Observable } from 'rxjs';
import { map, pairwise, shareReplay } from 'rxjs/operators';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.less']
})
export class AppComponent {
    private title = '';
    title$ = this.router.events.pipe(
        map(res => {
            this.title = res instanceof RoutesRecognized ? 'friendlizer: ' + res.state.root.firstChild?.data.title : this.title;
            return this.title;
        })
    );

    isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
        .pipe(
            map(result => result.matches),
            shareReplay()
        );

    constructor(private breakpointObserver: BreakpointObserver, private router: Router) { }
}
