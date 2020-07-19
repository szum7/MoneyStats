export class Common {
    public static ConsoleResponse(name: string, response: any): void {        
        console.log("=> " + name + ":");
        console.log(response);
        console.log("<=");
    }
}