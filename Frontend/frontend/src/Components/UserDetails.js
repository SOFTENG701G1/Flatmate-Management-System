import React from 'react';
import './UserDetails.css';
import cross from '../images/cross.png';
import edit from '../images/edit.png';
import cancel from '../images/delete.png';
import profile from '../images/user-profile.png';

export default class Members extends React.Component {
    constructor(props) {
        super(props);

    }

    render(){
        const showHideClassName = this.props.show ? "modal display-block" : "modal display-none";
        return (
            <div class="modal" className={showHideClassName}>
              <section className="modal-main">
                <input className="userDetailsButton" type="image" src= {cross} onClick={this.props.handleClose} />
                <img className="userDetailsImage" src={profile} />
                <h2 className="UserInformation" id="Name">{this.props.member.firstName} {this.props.member.lastName}</h2>
                <p className="InfoHeading">Username</p>
                <p className="UserInformation">{this.props.member.userName}</p>
                <p className="InfoHeading">Date of Birth</p>
                <p className="UserInformation">{this.props.member.dateOfBirth}</p>
                <p className="InfoHeading">Phone</p>
                <p className="UserInformation">{this.props.member.phoneNumber}</p>
                <p className="InfoHeading">Email</p>
                <p className="UserInformation">{this.props.member.email}</p>
                <p className="InfoHeading">Account Number</p>
                <p className="UserInformation">{this.props.member.bankAccount}</p>
                <p className="InfoHeading">Medical Conditions</p>
                <p className="UserInformation">{this.props.member.medicalInformation}</p>
                { this.props.isUser ? <div>
                    <input className="userDetailsButton" type="image" src= {edit} onClick={this.props.handleClose} />
                    <input className="userDetailsButton" type="image" src= {cancel} onClick={this.props.handleClose} /> </div>: ""}
              </section>
            </div>
          );
    }
}