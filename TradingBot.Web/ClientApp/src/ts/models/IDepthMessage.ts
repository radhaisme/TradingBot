import IOrderBook from "./IOrderBook";
import { SourceType } from "./SourceType";

export default interface IDepthMessage {
    readonly source: SourceType;
    readonly pair: string;
    readonly orderBook: IOrderBook;
}