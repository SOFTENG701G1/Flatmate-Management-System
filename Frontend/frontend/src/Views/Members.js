import React from 'react';
import NewFlat from '../Components/NewFlat';
import APIRequest from '../Util/APIRequest';
import { Redirect } from 'react-router-dom';
import User from '../Util/User';

export default class Members extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            flatMembers: ["temp"],
        }

        this.getMembers = this.getMembers.bind(this);
        this.getMembers();
    }

    async getMembers() {
        let resp = await APIRequest.getFlatMembers();
        let flat = await resp.json();
        console.log(flat)
        if (resp.ok) {
            User.setFlatState(flat);
        }
        this.forceUpdate();
    }

    render() {
        var flat = User.getFlatState();
        if (flat.flatMembers.length === 0) {
            return <Redirect to="/app/newFlat"/>
        } else {
            return <h2>Members page</h2>;
            
        }
    }
}