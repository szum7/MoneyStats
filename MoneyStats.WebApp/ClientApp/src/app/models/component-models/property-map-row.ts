export class PropertyMapRow {
    
    cell: string; // A1 | B2 | ...
    literal: string; // Könyvelés dátuma | Tranzakció azonosító  | ...
    property: string; // AccountingDate | TransactionId | ...
    parser: Function;

    isOpen: boolean;
    width: string;
    customClass: string;    

    constructor(cell: string, customClass: string, width: string, property: string, parser: Function) {

        this.cell = cell;
        this.customClass = customClass;
        this.property = property;
        this.parser = parser;
        
        this.width = width;
        this.isOpen = true;
    }
}