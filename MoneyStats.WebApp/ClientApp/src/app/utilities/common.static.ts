export class Common {
    public static ConsoleResponse(name: string, response: any): void {
        console.log("=> " + name + ":");
        console.log(response);
        console.log("<=");
    }

    public static containsObjectOnId(obj: any, list: any[]): boolean {
        for (let i = 0; i < list.length; i++) {
            if (list[i].id === obj.id) {
                return true;
            }
        }
        return false;
    }

    public static removeFromArray(obj: any, list: any[]): void {
        const index = list.indexOf(obj);
        if (index > -1) {
            list.splice(index, 1);
        }
    }
}