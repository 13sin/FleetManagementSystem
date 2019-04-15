package com.example.red.fleetworks;

import android.Manifest;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.location.Address;
import android.location.Geocoder;
import android.location.Location;
import android.os.Build;
import android.os.Looper;
import android.support.v4.app.ActivityCompat;
import android.support.v4.content.ContextCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import com.example.red.fleetworks.Data.Delivery;
import com.example.red.fleetworks.Data.Pickups;
import com.google.android.gms.location.FusedLocationProviderClient;
import com.google.android.gms.location.LocationCallback;
import com.google.android.gms.location.LocationRequest;
import com.google.android.gms.location.LocationResult;
import com.google.android.gms.location.LocationServices;
import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.MapView;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;
import com.pubnub.api.PNConfiguration;
import com.pubnub.api.PubNub;
import com.pubnub.api.callbacks.PNCallback;
import com.pubnub.api.models.consumer.PNPublishResult;
import com.pubnub.api.models.consumer.PNStatus;

import java.io.IOException;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;

public class ShipmentSelected extends AppCompatActivity {

    public static PubNub pubnub; // Pubnub instance
    Pickups pickup;
    Delivery delivery;
    private FusedLocationProviderClient mFusedLocationClient; // Object used to receive location updates

    private LocationRequest locationRequest; // Object that defines important parameters regarding location request.
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_shipment_selected);
        initPubnub();
        if (Build.VERSION.SDK_INT == Build.VERSION_CODES.M) {
            checkPermission();
        }
        final MapView mp = findViewById(R.id.mapView);
        mp.onCreate(savedInstanceState);
        Intent intent = getIntent();
        Bundle extras = intent.getExtras();
        final Geocoder geocoder = new Geocoder(this, Locale.US);
        if (extras != null) {
            if (extras.containsKey("pickup")) {
                pickup = (Pickups) intent.getSerializableExtra("pickup");
                final String staddress = pickup.getStreetname()+" "+pickup.getCity()+" "+pickup.getProvince()+" "+pickup.getZipcode();
                setmap(mp, geocoder, staddress);
            }else if(extras.containsKey("delivery")){
                delivery = (Delivery) intent.getSerializableExtra("delivery");
                final String staddress = delivery.getStreetname()+" "+delivery.getCity()+" "+delivery.getProvince()+" "+delivery.getZipcode();
                setmap(mp, geocoder, staddress);
            }
        }

        final Button startrout = findViewById(R.id.startroute);
        startrout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                mFusedLocationClient = LocationServices.getFusedLocationProviderClient(getApplicationContext());

                locationRequest = LocationRequest.create();
                locationRequest.setInterval(5000); // 5 second delay between each request
                locationRequest.setFastestInterval(5000); // 5 seconds fastest time in between each request
                locationRequest.setSmallestDisplacement(10); // 10 meters minimum displacement for new location request
                locationRequest.setPriority(LocationRequest.PRIORITY_HIGH_ACCURACY); // enables GPS high accuracy location requests

                sendUpdatedLocationMessage();
            }
        });
    }

    private void sendUpdatedLocationMessage() {
        Log.e("pub : " , "send update");
        try {
            Log.e("pub : " , "trying");
            mFusedLocationClient.requestLocationUpdates(locationRequest, new LocationCallback() {
                @Override
                public void onLocationResult(LocationResult locationResult) {
                    Log.e("pub : " , "atleadt requested");
                    Location location = locationResult.getLastLocation();
                    LinkedHashMap<String, String> message = getNewLocationMessage(location.getLatitude(), location.getLongitude());
                    ShipmentSelected.pubnub.publish()
                            .message(message)
                            .channel("fleetlocation")
                            .async(new PNCallback<PNPublishResult>() {
                                @Override
                                public void onResponse(PNPublishResult result, PNStatus status) {
                                    // handle publish result, status always present, result if successful
                                    // status.isError() to see if error happened
                                    if (!status.isError()) {
                                        Log.e("pub timetoken: " , result.getTimetoken().toString());
                                    }
                                    Log.e("pub status code: " , status.getStatusCode()+"");
                                }
                            });
                }
            }, Looper.myLooper());

        } catch (SecurityException e) {
            Log.e("pub : " , "something wrong not printing");
            e.printStackTrace();
        }
    }

    private void initPubnub() {
        PNConfiguration pnConfiguration = new PNConfiguration();
        pnConfiguration.setSubscribeKey("sub-c-6480e3a2-5766-11e9-ac27-a2b77a78ee25");
        pnConfiguration.setPublishKey("pub-c-7633429f-b38e-4a8e-a775-bda2d9fbd363");
        pnConfiguration.setSecretKey("sec-c-NzhkZjAxZTEtNjk2YS00NTgwLWE0MDItNWM5ZDY4OWZjNjNk");
        pnConfiguration.setSecure(true);
        pubnub = new PubNub(pnConfiguration);
    }

    public void checkPermission() {
        if (ContextCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED ||
                ContextCompat.checkSelfPermission(this, Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED
                ) {//Can add more as per requirement

            ActivityCompat.requestPermissions(this,
                    new String[]{Manifest.permission.ACCESS_FINE_LOCATION, Manifest.permission.ACCESS_COARSE_LOCATION},
                    123);
        }
    }

    private LinkedHashMap<String, String> getNewLocationMessage(double lat, double lng) {
        LinkedHashMap<String, String> map = new LinkedHashMap<String, String>();
        map.put("lat", String.valueOf(lat));
        map.put("lng", String.valueOf(lng));
        return map;
    }

    public void setmap(final MapView mp, final Geocoder geocoder, final String staddress){

        mp.getMapAsync(new OnMapReadyCallback() {
            @Override
            public void onMapReady(GoogleMap googleMap) {

                try{
                    List<Address> loc = geocoder.getFromLocationName(staddress, 5);
                    Address addr = loc.get(0);
                    //myLocation.setLatitude(addr.getLatitude());
                    //myLocation.setLongitude(addr.getLongitude());
                    googleMap.addMarker(new MarkerOptions().draggable(true).position(new LatLng(addr.getLatitude(), addr.getLongitude())).title("Marker"));
                    googleMap.moveCamera(CameraUpdateFactory.newLatLngZoom(new LatLng(addr.getLatitude(), addr.getLongitude()), 15));
                    mp.onResume();
                }
                catch(IOException e) {
                    Log.e("IOException", e.getMessage());
                    Toast.makeText(getApplicationContext(), "IOException:  " + e.getMessage(),Toast.LENGTH_LONG).show();
                }
            }
        });
    }
}
