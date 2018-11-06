import IDepthMessage from "./models/IDepthMessage";

export default interface IWebSocket {
    SubscribeToDepth(depth: number, symbols: string[], callback: (depth: IDepthMessage) => void): IWebSocket;
    Start(): IWebSocket;
    Stop(): IWebSocket;
}