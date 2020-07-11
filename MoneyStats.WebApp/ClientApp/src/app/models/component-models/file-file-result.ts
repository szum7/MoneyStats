import { ReadBankRowForDbCompare } from "src/app/models/component-models/read-bank-row-for-db-compare";
import { ExcelBankRowMapper } from "./excel-bank-row-mapper";

export class FileFileResult {
    public bankRowList: Array<ReadBankRowForDbCompare>;
    public mapper: ExcelBankRowMapper;

    constructor() {
        this.bankRowList = [];
    }
}