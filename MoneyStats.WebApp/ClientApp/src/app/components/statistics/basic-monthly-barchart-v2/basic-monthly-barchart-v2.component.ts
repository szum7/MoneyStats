import { Component, Input, Output, OnInit, HostListener } from '@angular/core';
import { StatisticsService } from 'src/app/services/statistics.service';
import { BasicMonthlyBarchart } from 'src/app/models/service-models/basic-monthly-barchart.model';
import { Common } from 'src/app/utilities/common.static';
import { Transaction } from 'src/app/models/service-models/transaction.model';
import { BasicMonthlyBar } from 'src/app/models/service-models/basic-monthly-bar.model';

class BarToggle {
    isIncomeHidden: boolean;
    isExpenseHidden: boolean;
    isFlowHidden: boolean;
}

@Component({
    selector: 'app-basic-monthly-barchart-v2-component',
    templateUrl: './basic-monthly-barchart-v2.component.html',
    styleUrls: ['./basic-monthly-barchart-v2.component.scss']
})
export class BasicMonthlyBarchartV2Component implements OnInit {

    data: BasicMonthlyBarchart;
    barToggle: BarToggle;

    maxHeight: number = 600;
    yUnits: Array<number>;
    unitTimes: number;
    isMouseInsideChart: boolean;
    crosshairHeight: number;
    crosshairWidth: number;
    crosshairLeft: number;
    crosshairTop: number;
    crosshairBottom: number;
    pageHeight: number = 885; // TODO

    @HostListener('document:mousemove', ['$event']) 
    onMouseMove(e) {
        if (this.isMouseInsideChart) {
            this.crosshairHeight = this.pageHeight - (e.y + (this.pageHeight - this.maxHeight)) + 30;
            this.crosshairWidth = e.x;
            this.crosshairTop = e.y;
            this.crosshairLeft = e.x;
            this.crosshairBottom = this.pageHeight - this.maxHeight;
        } else {
            this.crosshairHeight = 0;
            this.crosshairWidth = 0;
        }
        //console.log(e);
    }

    constructor(private statisticsService: StatisticsService) {
        this.yUnits = [];
        this.barToggle = new BarToggle();
        this.isMouseInsideChart = false;
    }

    ngOnInit(): void {
        let self = this;
        self.getBasicMonthlyBarchart(new Date(2010, 0, 1), new Date(2020, 2, 20), res => {
            self.data = res;
            self.initYUnits();
        });
    }

    mouseEnter() {
        this.isMouseInsideChart = true;
    }
    
    mouseLeave() {
        this.isMouseInsideChart = false;
    }

    toggleBar(name: string): void {
        if (name == "income") this.barToggle.isIncomeHidden = !this.barToggle.isIncomeHidden;
        if (name == "expense") this.barToggle.isExpenseHidden = !this.barToggle.isExpenseHidden;
        if (name == "flow") this.barToggle.isFlowHidden = !this.barToggle.isFlowHidden;
    }

    formatNumber(unit: number): string {
        return unit.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    }

    getUnitStepHeight(i: number): string {
        return (i * (this.maxHeight / this.unitTimes)) + "px";
    }

    getBarHeight(value: number): string {
        let ratio = this.maxHeight / this.data.maxValue;
        return Math.max(Math.abs(value) * ratio, 2) + "px";
    }

    getBarZIndex(bar: BasicMonthlyBar, current: string): number {
        let flow = Math.abs(bar.flow);
        let expense = Math.abs(bar.expense);
        if (bar.income >= expense && expense >= flow) switch (current) { case "income": return 1; case "expense": return 2; case "flow": return 3; };
        if (bar.income >= flow && flow >= expense) switch (current) { case "income": return 1; case "flow": return 2; case "expense": return 3; };
        if (flow >= expense && expense >= bar.income) switch (current) { case "flow": return 1; case "expense": return 2; case "income": return 3; };
        if (flow >= bar.income && bar.income >= expense) switch (current) { case "flow": return 1; case "income": return 2; case "expense": return 3; };
        if (expense >= bar.income && bar.income >= flow) switch (current) { case "expense": return 1; case "income": return 2; case "flow": return 3; };
        if (expense >= flow && flow >= bar.income) switch (current) { case "expense": return 1; case "flow": return 2; case "income": return 3; };
        console.error("getBarZIndex error.");
    }

    private initYUnits() {
        let unit: number = this.calculateYUnits(this.data.maxValue);
        this.unitTimes = this.data.maxValue / unit;
        for (let i = 0; i < this.unitTimes; i++) {
            this.yUnits.push(i * unit);
        }
    }

    private getBasicMonthlyBarchart(from: Date, to: Date, callback: (response: BasicMonthlyBarchart) => void): void {
        this.statisticsService.getBasicMonthlyBarchart(from, to).subscribe(res => {
            Common.ConsoleResponse("getBasicMonthlyBarchart", res);
            callback(res);
        }, err => {
            console.log(err);
        });
    }

    private calculateYUnits(max: number): number {
        /* Max          ||  Unit
         * 0 - 50       ->  1
         * 50 - 100     ->  5
         * 100 - 500    ->  10
         * 500 - 1000   ->  50
         * 1000 - 5000  ->  100
         * ...
         */
        let ret: number = 1;
        let cap: number = 50; // starting cap
        let c: number = 2;
        while (max > cap) {
            if (c % 2 === 0) {
                ret *= 5;
            } else {
                ret *= 2;
            }
            if (c % 2 === 0) {
                cap *= 2;
            } else {
                cap *= 5;
            }
            c++;
        }
        return ret;
    }
}
