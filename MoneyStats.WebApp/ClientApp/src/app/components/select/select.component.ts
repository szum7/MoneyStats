import { Component, Input, Output, EventEmitter, ViewEncapsulation } from '@angular/core';

// interface IEntity {
//     id: number;
//     title: string;
// }

@Component({
    selector: 'app-select-component',
    templateUrl: './select.component.html',
    styleUrls: ['./select.component.scss']
})
export class SelectComponent {

    // List of values
    @Input() setOfValues: any[];

    @Input() value: any;
    @Output() valueChange = new EventEmitter<any>();

    // Current autocomplete results
    results: any[];
    
    // Input string for the autocomplete
    str: string;

    constructor() {
        this.str = "";
        this.results = [];
    }

    click_close() {
        this.results = [];
    }

    click_select(item: any) {
        // Output value
        this.value = item;
        this.valueChange.emit(item);
        // Set input value for user to see
        this.str = item.title;
        // Hide results
        this.results = [];
    }

    change_getResults(): void {
        this.results = this.getResults(this.str);
    }

    private getResults(str: string): any[] {
        if (this.setOfValues.length === 0) {
            return [];
        }

        let ret: any[] = [];
        str = str.toLowerCase();

        let check: (str: string, tag: any) => boolean = null;
        if (!isNaN(Number(str))) { // id
            check = (str: string, tag: any) => Number(tag.id) === Number(str);
        } else { // title
            check = (str: string, tag: any) => tag.title.toLowerCase().includes(str);
        }

        this.setOfValues.forEach(tag => {
            if (check(str, tag)) {
                ret.push(tag);
            }
        });

        return ret;
    }
}
