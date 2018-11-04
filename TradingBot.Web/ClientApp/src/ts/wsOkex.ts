import IWebSocket from "./IWebSocket";
import IOrderBook from "./models/IOrderBook";
import IEvent from "./models/IEvent";
import IMessageData from "./models/IMessageData";

export default class wsOkex implements IWebSocket {
    private readonly _address: string = "wss://real.okex.com:10441/websocket";
    private readonly _ws: WebSocket = new WebSocket(this._address);

    public SubscribeToDepth(depth: number, symbols: string[], callback: (depth: IOrderBook) => void): IWebSocket {
        let request: IEvent = {
            event: "addChannel",
            channel: `ok_sub_spot_${symbols[0]}_depth`
        };
        // this._ws.send(JSON.stringify(request));

        return this;
    }

    private InitializeSocket(): void {
        this._ws.onopen = () => {
            console.log("opened.");
            let request: IEvent = {
                event: "addChannel",
                channel: `ok_sub_spot_${"btc_usdt"}_depth`
            };
            this._ws.send(JSON.stringify(request));
        };
        this._ws.onclose = () => {
            console.log("closed.");
        };
        this._ws.onerror = () => {
            console.log("error.");
        };
        this._ws.onmessage = (message: MessageEvent): void => {
            let data = <IMessageData>JSON.parse(message.data);
            // let position: number = data.stream.indexOf("@");
            // let stream: string = data.stream.substring(position);
            // this._handlers[stream](data);
            console.log(data.data);
        };
    }

    public Start(): IWebSocket {
        this.InitializeSocket();

        return this;
    }

    public Stop(): IWebSocket {
        this._ws.close();

        return this;
    }
}