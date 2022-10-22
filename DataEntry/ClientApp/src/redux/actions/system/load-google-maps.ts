export const loadGoogleMaps = (key:string, channel:string) => {
    if(!window.google){
        console.log("LOAD GOOGLE MAPS!");
        const script = document.createElement("script");
        script.type = "text/javascript";
        script.src = "https://maps.googleapis.com/maps/api/js?v=3&key=" + key + "&channel=" + channel + "&libraries=places";
        document.body.append(script);
    }
};