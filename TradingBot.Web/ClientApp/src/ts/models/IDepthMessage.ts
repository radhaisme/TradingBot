import IOrderBook from "./IOrderBook";

export default interface IDepthMessage {
    readonly source: string;
    readonly pair: string;
    orderBook: IOrderBook;
}