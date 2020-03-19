import React from 'react';
import UserImage from '../images/user-profile.png';

export default class MemberTile extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            person: props.flatMember,
            isUser: false,
        }

    }

    render(){
        var { person, isUser } = this.state;
        return(
            <div className="MemberProfile">
                <img className="userProfile" src={UserImage} />
                            <p> {person.firstName} {person.lastName} {isUser ? "(ME)" : ''}</p>
                            <p className="info"> Date of Birth: {person.dateOfBirth} </p>
                            <p className="info"> Phone Number: {person.phoneNumber}</p>
            </div>
        );
    }


}