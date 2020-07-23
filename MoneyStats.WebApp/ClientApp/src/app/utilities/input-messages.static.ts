export class StaticMessages {

    // When read multiple bank exported excel files and found a row duplication.
    public static READ_FILES_DUPLICATION: string = "Duplication found across multiple read files. We recommend to exclude this row from insertion!";
    // Toggleable exclusion/inclusion
    public static ROW_WILL_BE_EXCLUDED: string = "This row will be excluded.";
    // Matching BankRow detected after comparing the BankRow read from files and coming from the database.
    public static MATCHING_READ_BANKROW_WITH_DB: string = "There's already a BankRow with the same properties in the database. We recommend to exclude this row from insertion!";
    // When the generated transaction is modified from the original generated state (e.g.: a property was edited by the user)
    public static ROW_IS_MODIFIED: string = "You have modified this transaction from it's generated state!";
}