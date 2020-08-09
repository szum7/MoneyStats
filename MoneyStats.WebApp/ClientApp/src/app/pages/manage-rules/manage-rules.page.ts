import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-manage-rules-page',
    templateUrl: './manage-rules.page.html',
    styleUrls: ['./manage-rules.page.scss']
})
export class ManageRulesPage implements OnInit {

    isDeletionAllowed: boolean;
    isSavingAllowed: boolean;

    constructor() {
        this.isDeletionAllowed = false;
        this.isSavingAllowed = false;
    }

    ngOnInit(): void {
    }

    click_toggleDeleteSafeguard(): void {
        this.isDeletionAllowed = !this.isDeletionAllowed;
    }

    click_toggleSavingSafeguard(): void {
        this.isSavingAllowed = !this.isSavingAllowed;
    }
}
