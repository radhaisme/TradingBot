import IWebSocket from "./IWebSocket";
import IOrderBook from "./models/IOrderBook";

export default class wsBinance {
    private _ws: WebSocket;
    private readonly _baseAddress: string = "wss://stream.binance.com:9443";
    private readonly _limits: number[] = [5, 10, 20];
    private _streamUrl: string;
    private _messageHandler: (message: MessageEvent) => void;

    public SubscribeToDepth(depth: number, symbols: string[], callback?: (depth: IOrderBook) => void): wsBinance {
        if (!this._limits.includes(depth)) {
            throw Error(`Invalid range of depth.`);
        }

        if (!symbols) {
            throw Error(`The 'symbols' is undifined.`);
        }

        if (symbols.length === 0) {
            throw Error(`The 'symbols' did not provide.`);
        }

        let streams: string[] = symbols.map(symbol => `${symbol.toLowerCase()}@depth${depth}`);
        this._streamUrl = `${this._baseAddress}/stream?streams=${streams.join("/")}`;
        this._messageHandler = (message: MessageEvent): void => {
            let { data: depth } = JSON.parse(message.data);
            let orderBook: IOrderBook = {
                bids: depth.bids.map(([a, b]) => [+a, +b]),
                asks: depth.asks.map(([a, b]) => [+a, +b]),
            };

            if (callback) {
                callback(orderBook);
            }
        };

        return this;
    }

    private InitializeSocket(): void {
        console.log(this._streamUrl);
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
        this._ws.onmessage = this._messageHandler;
    }

    public Start(): wsBinance {
        if (!this._streamUrl || !this._messageHandler) {
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