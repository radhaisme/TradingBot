import IOrderBook from "./models/IOrderBook";

export default interface IWebSocket {
    SubscribeToDepth(depth: number, symbols: string[], callback: (depth: IOrderBook) => void): IWebSocket;
    Start(): IWebSocket;
    Stop(): IWebSocket;
}