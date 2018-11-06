import IRequest from "./IRequest";
import ITradePairsResponse from "./models/ITradePairsResponse";

export default class Connector {
    public async GetTradePairsAsync(request: IRequest): Promise<ITradePairsResponse> {
        var response = await fetch(`api/${request.apiName}/GetTradePairsAsync`, { method: "GET" });

        return response.json();
    }
}