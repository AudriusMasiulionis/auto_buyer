import { Typography, Button, Container, Card, CardContent, CardActions, Grid2 } from '@mui/material';

export default function Home() {
  return (
    <div>
      {/* Hero Section */}
      <Container sx={{ paddingY: 5, textAlign: 'center' }}>
        <Typography variant="h2" component="h1" gutterBottom>
          Welcome to MyApp
        </Typography>
        <Typography variant="body1" paragraph>
          Discover amazing content, stay updated, and explore the latest features we offer.
        </Typography>
        <Button variant="contained" color="primary" size="large" href="/form">
          Get Started
        </Button>
      </Container>

      <Container sx={{ paddingY: 5 }}>
        <Grid2 container spacing={4}>
          {Array.from(Array(3)).map((_, index) => (
            <Grid2 key={index} display="flex">
              <Card>
                <CardContent>
                  <Typography variant="h5" component="div" gutterBottom>
                    Feature {index + 1}
                  </Typography>
                  <Typography variant="body2">
                    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                  </Typography>
                </CardContent>
                <CardActions>
                  <Button size="small" color="primary" href={`/feature/${index + 1}`}>
                    Learn More
                  </Button>
                </CardActions>
              </Card>
            </Grid2>
          ))}
        </Grid2>
      </Container>
    </div>
  );
}
