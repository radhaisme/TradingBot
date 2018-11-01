export default interface IWebSocket {
    Subscribe(depth: number, ...symbols: string[]): void;
}