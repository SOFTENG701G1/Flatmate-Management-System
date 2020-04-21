import React from 'react';
import './UserProfileDetails.css';
import cross from '../images/cross.png';
import edit from '../images/edit.png';
import cancel from '../images/delete.png';
import profile from '../images/user-profile.png';
import Utils from '../Util/Utils'

export default class ProfileDetails extends React.Component {
    render(){
        
        return (
          <div className="userDetails">
                <img alt="user-profile" className="userDetailsImage" src={profile} />
                <h2 className="UserInformation" id="Name">{this.props.user.firstName} {this.props.user.lastName}</h2>
                <p className="InfoHeading">Username</p>
                <p className="UserInformation">{this.props.user.userName}</p>
                <p className="InfoHeading">Date of Birth</p>
                <p className="UserInformation">{Utils.dateFormatter(this.props.user.dateOfBirth)}</p>
                <p className="InfoHeading">Phone</p>
                <p className="UserInformation">{this.props.user.phoneNumber}</p>
                <p className="InfoHeading">Email</p>
                <p className="UserInformation">{this.props.user.email}</p>
                <p className="InfoHeading">Account Number</p>
                <p className="UserInformation">{this.props.user.bankAccount}</p>
                <p className="InfoHeading">Medical Conditions</p>
                <p className="UserInformation">{this.props.user.medicalInformation}</p>
                <div>
                    <input className="userDetailsButton" alt="edit-button" type="image" src= {edit} onClick={this.props.handleClose} />
              </div>
          </div>
          );
    }
}