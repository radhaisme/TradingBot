import IWebSocket from "./IWebSocket";
import IOrderBook from "./models/IOrderBook";
import IMessageData from "./models/IMessageData";

export default class wsBinance {
    private readonly _baseAddress: string = "wss://stream.binance.com:9443";
    private readonly _limits: number[] = [5, 10, 20];
    private readonly _handlers: { [key: string]: { (message: IMessageData): void } } = {};
    private _streamUrl: string;
    private _ws: WebSocket;

    public SubscribeToDepth(depth: number, symbols: string[], callback: (depth: IOrderBook) => void): wsBinance {
        if (!this._limits.includes(depth)) {
            throw Error(`Invalid range of depth.`);
        }

        if (!symbols) {
            throw Error(`The 'symbols' is undifined.`);
        }

        if (symbols.length === 0) {
            throw Error(`The 'symbols' did not provide.`);
        }

        if (!callback) {
            throw Error("The callbaack function is not provided.");
        }

        let stream: string = `@depth${depth}`;
        let streams: string[] = symbols.map(symbol => `${symbol.toLowerCase()}${stream}`);
        this._streamUrl = `${this._baseAddress}/stream?streams=${streams.join("/")}`;
        let local = (messageData: IMessageData): void => {
            let orderBook: IOrderBook = {
                bids: messageData.data.bids.map(([a, b]) => [+a, +b]),
                asks: messageData.data.asks.map(([a, b]) => [+a, +b]),
            };

            if (callback) {
                callback(orderBook);
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

    public Start(): wsBinance {
        if (Object.keys(this._handlers).length === 0) {
            throw Error("The socket is not initialized.");
        }

        this.InitializeSocket();

        return this;
    }

    public Stop(): wsBinance {
        if (this._ws) {
            this._ws.close();
        }

        return this;
    }
}