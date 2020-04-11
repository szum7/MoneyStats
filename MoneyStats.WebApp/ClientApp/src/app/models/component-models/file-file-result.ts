import { ReadBankRowForInsertion } from "src/app/models/component-models/read-bank-row-for-insertion";
import { ExcelBankRowMapper } from "./excel-bank-row-mapper";

export class FileFileResult {
    public bankRowList: Array<ReadBankRowForInsertion>;
    public mapper: ExcelBankRowMapper;

    constructor() {
        this.bankRowList = [];
    }
}