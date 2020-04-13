export default class Utils {
    
    static dateFormatter(date){
        var date = new Date(date);
        return date.getDay() + "/" + date.getMonth() + "/" + date.getFullYear();
    }

}