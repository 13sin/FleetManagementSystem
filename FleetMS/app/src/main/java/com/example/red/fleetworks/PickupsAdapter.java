package com.example.red.fleetworks;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.example.red.fleetworks.Data.Pickups;

import java.util.List;

class PickupsAdapter extends ArrayAdapter {
    private final Activity context;
    List<Pickups> pickups;

    public PickupsAdapter(Activity context, List<Pickups> Shiftlist){
        super(context,R.layout.pickup_list_row , Shiftlist);
        this.pickups = Shiftlist;
        this.context = context;
    }

    public View getView(int position, View view, ViewGroup parent) {
        LayoutInflater inflater=context.getLayoutInflater();
        View rowView=inflater.inflate(R.layout.pickup_list_row, null,true);

        TextView pickuplocation = (TextView) rowView.findViewById(R.id.pickuplocation);
        TextView pickuptime = (TextView) rowView.findViewById(R.id.pickuptime);

        pickuplocation.setText("Location: "+pickups.get(position).getStreetname()+ " "+pickups.get(position).getCity()+" "+pickups.get(position).getProvince());
        pickuptime.setText("Time: "+pickups.get(position).getTime());
        return rowView;
    }
}
