// These responses are unused!

export abstract class BaseResponse<T> {
    public isValid: boolean;
    public messages: Array<string>;
    public data: T;

    isValidResponse(): boolean {
        return this.isValid && (this.messages == null || this.messages.length === 0);
    }
}

class EmptyObject {
}

/**
 * A model for actions which don't require data, only a success/error respone (sent in "isValid")
 */
export class EmptyResponse extends BaseResponse<EmptyObject> {
}

export class MessageResponse extends BaseResponse<string> {    
}

export class CountResponse extends BaseResponse<number> {    
}
  