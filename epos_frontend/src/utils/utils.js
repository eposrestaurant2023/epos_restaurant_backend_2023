import { createResource } from '@/plugin'
export function utils(){
    // async function    isWorkingDayOpened() {
    //     await createResource({
    //         url:"epos_restaurant_2023.api.api.get_current_working_day",
    //         params:{
    //             pos_profile: localStorage.getItem("pos_profile")
    //         },
    //         auto:true,
    //         onSuccess(d){
    //             if(d==undefined){
    //                 return false 
    //             }else {
    //                 return d;
    //             }
    //         }
    //     })
    // }

    function isCashierShiftOpened(){
        return false; 
    }

    return {
            isWorkingDayOpened,
            isCashierShiftOpened
        }
}