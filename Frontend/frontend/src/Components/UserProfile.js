import React from 'react';
import APIRequest from '../Util/APIRequest';
import ProfileDetails from './UserProfileDetails';


export default class ProfilePage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            user:  null,
            isLoaded: false,
        }
        this.getUserInfo();
    }



    async getUserInfo(){
        const userResult = await APIRequest.getUserInfo()
        const json = await userResult.json();
        this.setState({
            isLoaded: true,
            user: json
        })
    }



    render() {
        var { isLoaded, user } = this.state;

        if (!isLoaded) {
            return (<div>Loading...</div>);
        }
        else{
            return (
                <div className = "ProfilePage">

                            <div >
                                <div className='section-header'>{user.userName}</div>
                                <ProfileDetails user={user} />
                            </div>
                </div>)
        }
    }



}
