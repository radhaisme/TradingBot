import IWebSocket from "./IWebSocket";
import IOrderBook from "./models/IOrderBook";
import IMessageData from "./models/IMessageData";
import IDictionary from "./models/IDictionary";
import IDepthMessage from "./models/IDepthMessage";

export default class wsBinance implements IWebSocket {
    private readonly _baseAddress: string = "wss://stream.binance.com:9443";
    private readonly _limits: number[] = [5, 10, 20];
    private readonly _handlers: { [key: string]: { (message: IMessageData): void } } = {};
    private readonly _pairs: IDictionary<string> = {};
    private _streamUrl: string;
    private _ws: WebSocket;

    public SubscribeToDepth(depth: number, pairs: string[], callback: (depth: IDepthMessage) => void): IWebSocket {
        if (!this._limits.includes(depth)) {
            throw Error(`Invalid range of depth.`);
        }

        if (!pairs) {
            throw Error(`The 'pairs' is undifined.`);
        }

        if (pairs.length === 0) {
            throw Error(`The 'pairs' did not provide.`);
        }

        if (!callback) {
            throw Error("The callbaack function is not provided.");
        }

        let stream: string = `@depth${depth}`;
        let streams: string[] = [];
        pairs.forEach(pair => {
            let pairStream: string = `${pair.replace("/", "").toLowerCase()}${stream}`;
            this._pairs[pairStream] = pair;
            streams.push(pairStream);
        });
        this._streamUrl = `${this._baseAddress}/stream?streams=${streams.join("/")}`;
        let local = (messageData: IMessageData): void => {
            let orderBook: IOrderBook = {
                bids: messageData.data.bids.map(([a, b]) => [+a, +b]),
                asks: messageData.data.asks.map(([a, b]) => [+a, +b]),
            };
            let message: IDepthMessage = {
                source: "Binance",
                pair: this._pairs[messageData.stream],
                orderBook: orderBook,
            };

            if (callback) {
                callback(message);
            }
        };
        this._handlers[stream] = local;

        return this;
    }

    private InitializeSocket(): void {
        this._ws = new WebSocket(this._streamUrl);
        this._ws.onopen = () => {
            console.log("opened.");
        };
        this._ws.onclose = () => {
            console.log("closed.");
        };
        this._ws.onerror = () => {
            console.log("error.");
        };
        this._ws.onmessage = (message: MessageEvent): void => {
            let data = <IMessageData>JSON.parse(message.data);
            let position: number = data.stream.indexOf("@");
            let stream: string = data.stream.substring(position);
            this._handlers[stream](data);
        };
    }

    public Start(): IWebSocket {
        if (Object.keys(this._handlers).length === 0) {
            throw Error("The socket is not initialized.");
        }

        this.InitializeSocket();

        return this;
    }

    public Stop(): IWebSocket {
        if (this._ws) {
            this._ws.close();
        }

        return this;
    }
}