export const hexToRgb = (hex:any) => {
    if(hex.substr(0,1) === '#'){
        hex = hex.substr(1,hex.length);
    }
    if(hex.length !== 6){
        return;
    }
    const aRgbHex = hex.match(/.{1,2}/g);
    const aRgb = [
        parseInt(aRgbHex[0], 16),
        parseInt(aRgbHex[1], 16),
        parseInt(aRgbHex[2], 16)
    ];
    return "rgb(" + aRgb[0] + ", " + aRgb[1] + ", " + aRgb[2] + ")";
}
