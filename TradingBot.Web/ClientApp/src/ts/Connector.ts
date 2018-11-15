import IRequest from "./IRequest";
import ITradePairsResponse from "./models/ITradePairsResponse";
import TradePair from "./models/TradePair";

export default class Connector {
    public async GetTradePairsAsync(request: IRequest): Promise<ITradePairsResponse> {
        let response = await fetch(`api/${request.apiName}/GetTradePairsAsync`, { method: "GET" });

        return response.json();
    }
}