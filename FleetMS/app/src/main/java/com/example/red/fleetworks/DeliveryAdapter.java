package com.example.red.fleetworks;

import android.app.Activity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.example.red.fleetworks.Data.Delivery;
import com.example.red.fleetworks.Data.Pickups;

import java.util.List;

class DeliveryAdapter extends ArrayAdapter {
    private final Activity context;
    List<Delivery> deliveries;

    public DeliveryAdapter(Activity context, List<Delivery> deliveries){
        super(context,R.layout.pickup_list_row , deliveries);
        this.deliveries = deliveries;
        this.context = context;
    }

    public View getView(int position, View view, ViewGroup parent) {
        LayoutInflater inflater=context.getLayoutInflater();
        View rowView=inflater.inflate(R.layout.pickup_list_row, null,true);

        TextView pickuplocation = (TextView) rowView.findViewById(R.id.pickuplocation);
        TextView pickuptime = (TextView) rowView.findViewById(R.id.pickuptime);

        pickuplocation.setText("Location: "+deliveries.get(position).getStreetname()+ " "+deliveries.get(position).getCity()+" "+deliveries.get(position).getProvince());
        pickuptime.setText("Time: "+deliveries.get(position).getTime());
        return rowView;
    }
}
