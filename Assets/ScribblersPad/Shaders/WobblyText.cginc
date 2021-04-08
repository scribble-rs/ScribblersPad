float4 GetWonkyTextOffset(float2 position, float2 weight)
{
	const float2 a_array[3] = { float2(12.9898, 6582.4383), float2(664.561654, 64964.78564), float2(634.6415, 75.6328) };
	const float2 b_array[3] = { float2(78.233, 942.6324), float2(561.651654, 9654.62747), float2(75.64645, 8746.6655) };
	const float2 c_array[3] = { float2(43758.5453, 61554.5526), float2(65475.54651, 8956.64587), float2(546.5317, 7462.6526) };

	int index = (abs(_Time.y) * 8.0) % 3;

	float2 a = a_array[index];
	float2 b = b_array[index];
	float2 c = c_array[index];

	float2 dt = float2(dot(position, float2(a.x, b.x)), dot(position, float2(a.y, b.y)));
	float2 sn = float2(fmod(dt.x, 3.14), fmod(dt.y, 3.14));
	return float4(frac(sin(sn.x) * c.x) * weight.x, frac(sin(sn.y) * c.y) * weight.y, 0.0, 0.0);
}
