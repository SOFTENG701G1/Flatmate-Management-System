import React from 'react';
import UserImage from '../images/user-profile.png';
import UserDetails from './UserDetails'
import Utils from '../Util/Utils'

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
            <temp>
                <div className="MemberProfile" onClick={this.showModal}>
                    <img alt="user-profile" className="userProfile" src={UserImage} />
                    <p className="name"> {person.firstName} {person.lastName} {isUser ? "(ME)" : ''}</p>
                    <p className="info"> Date of Birth: {Utils.dateFormatter(person.dateOfBirth)} </p>
                    <p className="info"> Phone Number: {person.phoneNumber}</p>

                </div>
                <UserDetails show={show} handleClose={this.hideModal} member={person} />
            </temp>
                
        );
    }


}