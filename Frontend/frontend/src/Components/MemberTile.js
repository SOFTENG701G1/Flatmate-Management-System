import React from 'react';
import UserImage from '../images/user-profile.png';
import UserDetails from './UserDetails'

export default class MemberTile extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            person: props.flatMember,
            isUser: false,
            show: false
        }
        this.showModal = this.showModal.bind(this);
        this.hideModal = this.hideModal.bind(this);
    }

    showModal() {
        this.setState({ show: true });
    }

    hideModal() {
        this.setState({ show: false });
    }

    render() {
        var { person, isUser, show } = this.state;
        return (
            <div>
                <div className="MemberProfile" onClick={this.showModal}>
                    <img className="userProfile" src={UserImage} />
                    <p className="name"> {person.firstName} {person.lastName} {isUser ? "(ME)" : ''}</p>
                    <p className="info"> Date of Birth: {person.dateOfBirth} </p>
                    <p className="info"> Phone Number: {person.phoneNumber}</p>

                </div>
                <UserDetails show={show} handleClose={this.hideModal} member={person} />
            </div>
                
        );
    }


}