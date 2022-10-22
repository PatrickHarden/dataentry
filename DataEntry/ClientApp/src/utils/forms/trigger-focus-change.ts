/* this is a function we can call when needed to force the UI to leave focus on the current field */
export const triggerFocusChange = () => {
    const el:any = document.querySelector(':focus');
    if(el){
        el.blur();
    } 
}