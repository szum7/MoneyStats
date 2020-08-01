import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Common } from 'src/app/utilities/common.static';

interface IEntity {
    id: number;
    title: string;
}

@Component({
    selector: 'app-dropdown-component',
    templateUrl: './dropdown.component.html',
    styleUrls: ['./dropdown.component.scss']
})
export class DropdownComponent {

    // List of values
    @Input() setOfValues: any[];

    // Values already selected on the outside component
    // These values will not appear in the autocomplete
    @Input() value: any[];

    // Select value event
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
        this.value.push(item);
        this.valueChange.emit(this.value);

        Common.removeFromArray(item, this.results);
    }

    change_getResults(): void {
        this.results = this.getResults(this.str, this.value);
    }

    private getResults(str: string, excludes: any[]): any[] {
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
                if (!Common.containsObjectOnId(tag, excludes)) {
                    ret.push(tag);
                }
            }
        });

        return ret;
    }
}
