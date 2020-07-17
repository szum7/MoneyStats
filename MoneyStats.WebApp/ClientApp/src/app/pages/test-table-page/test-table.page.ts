import { Component, OnInit } from '@angular/core';
import { TestTableService } from '../../services/test-table-service/test-table.service';

@Component({
    selector: 'app-test-table-page',
    templateUrl: './test-table.page.html'
})
export class TestTablePage implements OnInit {

    constructor(private testTableService: TestTableService) {
    }

    ngOnInit(): void {
        this.testPost();
    }

    private testPost(): void {
        let data = {
            id: 123,
            names: ["a", "b", "c"]
        };

        this.testTableService.postTest(data).subscribe(response => {
            console.log(response);
        }, error => {
            console.log(error);
        });
    }

    public get(): void {
        this.testTableService.get().subscribe((response) => {
            console.log(response);
        }, (error) => {
            console.log(error);
        });
    }

    public testCall(): void {
        this.testTableService.testCall().subscribe((response) => {
            console.log(response);
        }, (error) => {
            console.log(error);
        });
    }
}
