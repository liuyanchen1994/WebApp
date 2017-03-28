export interface Model {
    id: string;
    createdTime: Date;
    status: number;
    updateTime: Date;
}



export interface JResponse<T> {
    data: T;
    dateTime: Date;
    errorCode: number;
    msg: string;
}