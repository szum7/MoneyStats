import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-manage-rules-page',
    templateUrl: './manage-rules.page.html',
    styleUrls: ['./manage-rules.page.scss']
})
export class ManageRulesPage implements OnInit {

    isDeletionAllowed: boolean;

    constructor() {
        this.isDeletionAllowed = false;
    }

    ngOnInit(): void {
    }

    click_toggleDeleteSafeguard(): void {
        this.isDeletionAllowed = !this.isDeletionAllowed;
    }
}
